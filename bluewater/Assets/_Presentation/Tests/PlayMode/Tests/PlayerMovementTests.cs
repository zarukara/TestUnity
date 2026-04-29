using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerMovementTests
{
    private const float MovementThreshold = 0.01f;

    private static readonly MethodInfo MoveMethod = typeof(PlayerMovement).GetMethod(
        "Move",
        BindingFlags.Instance | BindingFlags.NonPublic);

    [UnityTest]
    public IEnumerator Move_WithForwardInput_MovesCharacterForward()
    {
        PlayerMovement movement = CreatePlayerMovement(out GameObject playerObject);
        Vector3 startPosition = playerObject.transform.position;

        InvokeMove(movement, Vector3.forward);
        yield return null;

        Assert.Greater(
            playerObject.transform.position.z,
            startPosition.z + MovementThreshold,
            "Персонаж должен смещаться вперёд при движении по оси Z.");

        Object.DestroyImmediate(playerObject);
    }

    [UnityTest]
    public IEnumerator Move_WithoutInput_DoesNotChangeCharacterPosition()
    {
        PlayerMovement movement = CreatePlayerMovement(out GameObject playerObject);
        Vector3 startPosition = playerObject.transform.position;

        InvokeMove(movement, Vector3.zero);
        yield return null;

        Assert.That(
            Vector3.Distance(startPosition, playerObject.transform.position),
            Is.LessThanOrEqualTo(MovementThreshold),
            "Без входного движения персонаж не должен смещаться.");

        Object.DestroyImmediate(playerObject);
    }

    [UnityTest]
    public IEnumerator Move_WithForwardInputAndRotation_MovesInFacingDirection()
    {
        PlayerMovement movement = CreatePlayerMovement(out GameObject playerObject);
        playerObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        Vector3 startPosition = playerObject.transform.position;

        InvokeMove(movement, Vector3.forward);
        yield return null;

        Assert.Greater(
            playerObject.transform.position.x,
            startPosition.x + MovementThreshold,
            "После поворота персонаж должен идти вперёд относительно своего направления.");

        Object.DestroyImmediate(playerObject);
    }

    private static PlayerMovement CreatePlayerMovement(out GameObject playerObject)
    {
        playerObject = new GameObject("Test Player");
        playerObject.transform.position = new Vector3(0f, 1f, 0f);

        CharacterController controller = playerObject.AddComponent<CharacterController>();
        controller.height = 2f;
        controller.radius = 0.5f;
        controller.center = new Vector3(0f, 1f, 0f);

        PlayerMovement movement = playerObject.AddComponent<PlayerMovement>();
        movement.enabled = false;
        movement.speed = 6f;
        movement.gravity = -9.81f;
        movement.jumpHeight = 1.5f;

        FieldInfo controllerField = typeof(PlayerMovement).GetField(
            "controller",
            BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.IsNotNull(controllerField, "Не удалось получить поле controller у PlayerMovement.");
        controllerField.SetValue(movement, controller);
        return movement;
    }

    private static void InvokeMove(PlayerMovement movement, Vector3 input)
    {
        Assert.IsNotNull(MoveMethod, "Не удалось найти приватный метод Move у PlayerMovement.");
        MoveMethod.Invoke(movement, new object[] { input });
    }
}
